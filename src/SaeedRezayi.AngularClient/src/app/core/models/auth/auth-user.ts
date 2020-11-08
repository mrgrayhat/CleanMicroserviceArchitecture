export interface AuthUser {
  userId: string;
  userName: string;
  profilePicture:string;
  displayName: string;
  roles: string[] | null;
}
