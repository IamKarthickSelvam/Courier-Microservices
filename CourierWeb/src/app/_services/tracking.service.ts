import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Tracking } from '../_models/tracking';

@Injectable({
  providedIn: 'root'
})
export class TrackingService {
  private trackingDetails = new BehaviorSubject<Tracking | null>(null);
  trackingDetails$ = this.trackingDetails.asObservable();

  baseUrl: string = 'https://localhost:6001/api/Tracking';

  constructor(private http: HttpClient) { }

  getBooking(bookingId: number | undefined): Observable<any> {
    return this.http.get<Tracking>(this.baseUrl + '/BookingId/' + `${bookingId}`);
  }

  setTrackingData(tracking: Tracking | null) {
    this.trackingDetails.next(tracking);
    localStorage.setItem('tracking', JSON.stringify(tracking));
  }
}