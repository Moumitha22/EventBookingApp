export type UserRole = 'User';

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  role: UserRole;
}
