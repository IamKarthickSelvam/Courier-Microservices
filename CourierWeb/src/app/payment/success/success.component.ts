import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BookingService } from 'src/app/_services/booking.service';
import { PaymentService } from 'src/app/_services/payment.service';

@Component({
  selector: 'app-success',
  templateUrl: './success.component.html',
  styleUrls: ['./success.component.css']
})
export class SuccessComponent {
  booking: any = {};
  payment: any;

  constructor(
    private bookingService: BookingService,
    private paymentService: PaymentService,
    private route: ActivatedRoute) {
    this.getDetails()
    this.updatePaymentStatus();
  }

  getDetails() {
    if (localStorage.getItem('booking') == null) {
      if (this.booking == null) {
        this.bookingService.bookingDetails$.subscribe({
          next: response => this.booking = response
        });
      }
    }
    else {
      this.booking = JSON.parse(localStorage['booking']);
    }
    console.log(this.booking.id);


    if (localStorage.getItem('stripe') == null) {
      if (this.payment == null) {
        this.paymentService.paymentDetails$.subscribe({
          next: response => this.payment = response,
          error: error => console.log(error),
          complete: () => console.log('Successfully retrieved Stripe object')
        });
      }
    }
    else {
      this.payment = JSON.parse(localStorage['stripe']);
    }
  }

  updatePaymentStatus() {
    this.paymentService.confirmPayment(this.payment);
  }
}
