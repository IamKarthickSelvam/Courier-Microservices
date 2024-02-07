import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { Booking } from '../_models/booking';
import { Pincodes } from '../_models/pincodes';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private bookingDetails = new BehaviorSubject<Booking | null | undefined>(null);
  bookingDetails$ = this.bookingDetails.asObservable();

  baseUrl: string = 'https://localhost:2001/api/Booking';

  constructor(private http: HttpClient) { }

  getPincodes(): Observable<any> {
    return this.http.get<Pincodes>(this.baseUrl + '/Pincode');
  }

  createBooking(booking: Booking): Observable<Booking> {
    return this.http.post<Booking>(this.baseUrl, booking);
  }

  setBooking(booking: Booking | null) {
    this.bookingDetails.next(booking);
    localStorage.setItem('booking', JSON.stringify(booking));
  }
}
