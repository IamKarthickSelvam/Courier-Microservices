import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingService } from 'src/app/_services/booking.service';
import { PaymentService } from 'src/app/_services/payment.service';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css']
})
export class LoadingComponent {
  booking: any;
  stripe: any = {};

  constructor(
    private bookingService: BookingService,
    private paymentService: PaymentService,
    private router: Router) {
    this.getDetails();
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

    if (localStorage.getItem('stripe') == null) {
      if (this.stripe == null) {
        this.paymentService.paymentDetails$.subscribe({
          next: response => this.stripe = response
        });
      }
    }
    else {
      this.stripe = JSON.parse(localStorage['stripe']);
    }
    console.log(this.stripe);
    
    this.changeView();
  }

  changeView() {
    this.paymentService.createCheckoutSession(this.booking).subscribe({
      next: response => {
        this.stripe.id = response.id;
        this.stripe.url = response.url;
        localStorage.setItem('stripe', JSON.stringify(this.stripe));
        setTimeout(() => {
          window.location.href = response.url;
        }, 3000);
      },
      error: error => console.log(error),
      complete: () => console.log('Successfully received Stripe session details')
    });
  }
}
