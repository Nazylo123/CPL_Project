export interface User {
  userName: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  accessFailedCount: boolean;
}
export class UserRequestViewModel {
  userName: string = '';
  fullName: string = '';
  email: string = '';
  phoneNumber: string = '';
  accessFailedCount: boolean = false;
}
