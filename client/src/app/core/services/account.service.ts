import { HttpClient, HttpParams } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Address, User } from '../../shared/models/user';
import { map, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:5001/api/';
  currentUser = signal<User | null>(null);
  isAdmin = computed(() => {
    const roles = this.currentUser()?.roles;

    return Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin';
  });

  login(values: any) {
    let params = new HttpParams();
    params = params.append('useCookies', true);

    return this.http.post<User>(this.baseUrl + 'login', values, {
      params,
    });
  }

  register(values: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', values);
  }

  getUserInfo() {
    return this.http.get<User>(this.baseUrl + 'account/user-info').pipe(
      map((user) => {
        this.currentUser.set(user);

        return user;
      })
    );
  }

  logout() {
    return this.http.post(this.baseUrl + 'account/logout', {});
  }

  updateAddress(address: Address) {
    return this.http.post(this.baseUrl + 'account/address', address).pipe(
      tap(() => {
        this.currentUser.update((user) => {
          if (user) user.address = address;

          return user;
        });
      })
    );
  }

  getAuthState() {
    return this.http.get<{ isAuthenticated: boolean }>(
      this.baseUrl + 'account/auth-status'
    );
  }
}
