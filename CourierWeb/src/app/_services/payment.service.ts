import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { Booking } from '../_models/booking';
import { Stripe } from '../_models/stripe';

@Injectable({
    providedIn: 'root'
})
export class PaymentService {
    private paymentDetails = new BehaviorSubject<Stripe | null>(null);
    paymentDetails$ = this.paymentDetails.asObservable();

    baseUrl: string = 'https://localhost:4001/api/Payment';

    constructor(private http: HttpClient) { }

    createCheckoutSession(booking: Booking): Observable<Stripe> {
        return this.http.post<Stripe>(this.baseUrl, booking);
    }

    confirmPayment(stripeSession: Stripe): Observable<any> {
        return this.http.post(this.baseUrl + '/ConfirmPayment', stripeSession);
    }

    setPayment(payment: Stripe | null) {
        this.paymentDetails.next(payment);
        localStorage.setItem('stripe', JSON.stringify(payment));
    }
}
