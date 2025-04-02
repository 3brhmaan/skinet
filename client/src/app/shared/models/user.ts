export type User = {
  firstName: string;
  lastName: string;
  email: string;
  address: Address;
};

export type Address = {
  lin1: string;
  lin2: string;
  city: string;
  country: string;
  postalCode: string;
};
