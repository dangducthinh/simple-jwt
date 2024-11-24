import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenKey: string = 'accessToken';
  private apiUrl = 'http://localhost:5235/api/auth';  // Replace with your API URL

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<void> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = { username, password };

    return this.http.post<{ accessToken: string }>(`${this.apiUrl}/login`, body, { headers })
      .pipe(
        map(response => {
          localStorage.setItem(this.tokenKey, response.accessToken);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return localStorage.getItem(this.tokenKey) !== null;
  }
}
