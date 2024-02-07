import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingService } from 'src/app/_services/booking.service';
import { PaymentService } from 'src/app/_services/payment.service';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent {
  booking: any;
  stripe: any = {};

  constructor(
    private bookingService: BookingService,
    private paymentService: PaymentService,
    private router: Router,
    private route: ActivatedRoute) {
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
  }

  changeView() {
    this.bookingService.createBooking(this.booking).subscribe({
      next: response => {
        console.log(response);
        this.booking = response;
        this.stripe.bookingId = response.id;
        this.paymentService.setPayment(this.stripe);
        console.log(response);
        console.log(response.id);
        console.log(this.stripe);

        this.router.navigate(['/payment/loading'], { relativeTo: this.route });
      },
      error: error => console.log(error),
      complete: () => console.log('Successfully received Stripe object for Payment'),
    });

  }
}
