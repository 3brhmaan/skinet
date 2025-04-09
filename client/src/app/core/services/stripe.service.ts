import { inject, Injectable } from '@angular/core';
import {
  ConfirmationToken,
  loadStripe,
  Stripe,
  StripeAddressElement,
  StripeAddressElementOptions,
  StripeElements,
  StripePaymentElement,
} from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CartService } from './cart.service';
import { Cart } from '../../shared/models/cart';
import { firstValueFrom, map } from 'rxjs';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root',
})
export class StripeService {
  private baseUrl = environment.apiUrl;
  private cartService = inject(CartService);
  private http = inject(HttpClient);
  private stripePromise: Promise<Stripe | null>;
  private elements?: StripeElements;
  private addressElement?: StripeAddressElement;
  private accountService = inject(AccountService);
  private paymentElement?: StripePaymentElement;

  constructor() {
    this.stripePromise = loadStripe(environment.stripePublicKey);
  }

  getStripeInstance() {
    return this.stripePromise;
  }

  async initializeElements() {
    if (!this.elements) {
      const strip = await this.getStripeInstance();

      if (strip) {
        const cart = await firstValueFrom(this.createOrUpdatePaymentIntent());
        this.elements = strip.elements({
          clientSecret: cart.clientSecret,
          appearance: { labels: 'floating' },
        });
      } else {
        throw new Error('Stripe has not loaded');
      }
    }

    return this.elements;
  }

  async createPaymentElement() {
    if (!this.paymentElement) {
      const element = await this.initializeElements();

      if (element) {
        this.paymentElement = this.elements?.create('payment');
      } else {
        throw new Error('Elements instance has not initialized');
      }
    }

    return this.paymentElement;
  }

  async createAddressElement() {
    if (!this.addressElement) {
      const elements = await this.initializeElements();
      if (elements) {
        const user = this.accountService.currentUser();
        let defaultValues: StripeAddressElementOptions['defaultValues'] = {};
        if (user) {
          defaultValues.name = user.firstName + ' ' + user.lastName;
        }

        if (user?.address) {
          defaultValues.address = {
            line1: user.address.line1,
            line2: user.address.line2,
            city: user.address.city,
            state: user.address.state,
            country: user.address.country,
            postal_code: user.address.postalCode,
          };
        }

        const options: StripeAddressElementOptions = {
          mode: 'shipping',
          defaultValues: defaultValues,
        };

        this.addressElement = elements.create('address', options);
      } else {
        throw new Error('Elements instance has not loaded');
      }
    }

    return this.addressElement;
  }

  async createConfirmationToken() {
    const stripe = await this.getStripeInstance();
    const elements = await this.initializeElements();

    const result = await elements.submit();
    if (result.error) throw new Error(result.error.message);

    if (stripe) {
      return await stripe.createConfirmationToken({ elements });
    } else {
      throw new Error('Stripe not available');
    }
  }

  async confirmPayment(confirmationToken: ConfirmationToken) {
    const stripe = await this.getStripeInstance();
    const elements = await this.initializeElements();

    const result = await elements.submit();
    if (result.error) throw new Error(result.error.message);

    const clientSecret = this.cartService.cart()?.clientSecret;
    if (stripe && clientSecret) {
      return await stripe.confirmPayment({
        clientSecret: clientSecret,
        confirmParams: {
          confirmation_token: confirmationToken.id,
        },
        redirect: 'if_required',
      });
    } else {
      throw new Error('unable to load stripe');
    }
  }

  createOrUpdatePaymentIntent() {
    const cart = this.cartService.cart();
    if (!cart) throw new Error('Problem With Cart');

    return this.http.post<Cart>(this.baseUrl + 'payments/' + cart.id, {}).pipe(
      map((cart) => {
        this.cartService.setCart(cart);

        return cart;
      })
    );
  }

  disposeElement() {
    this.elements = undefined;
    this.addressElement = undefined;
    this.paymentElement = undefined;
  }
}
