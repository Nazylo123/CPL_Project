import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { User, UserRequestViewModel } from './Model/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'https://localhost:7102/api/Users'; // Thay URL đúng cho API

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }

  getUserByEmail(email: string | null): Observable<any> {
    return this.http
      .get<any>(`${this.apiUrl}/${email}`)
      .pipe(catchError(this.handleError));
  }
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // Lỗi phía client
      errorMessage = `Client error: ${error.error.message}`;
    } else {
      // Lỗi phía server
      errorMessage = `Server error: ${error.status}, ${error.message}`;
    }
    return throwError(errorMessage);
  }

  updateUser(
    email: string | null,
    user: UserRequestViewModel
  ): Observable<any> {
    return this.http.put(`${this.apiUrl}/${email}`, user);
  }
}
