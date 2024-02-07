import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingService } from 'src/app/_services/booking.service';

@Component({
  selector: 'app-amount',
  templateUrl: './amount.component.html',
  styleUrls: ['./amount.component.css']
})
export class AmountComponent {
  booking: any = {};
  stripe: any;

  amount1: number = 0;
  amount2: number = 0;
  amount3: number = 0;
  amount4: number = 0;

  constructor(private bookingService: BookingService,
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
    this.amount1 = Math.floor(this.booking.value * 0.2);
    this.amount2 = Math.floor(this.booking.value * 0.3);
    this.amount3 = Math.floor(this.booking.value * 0.4);
    this.amount4 = Math.floor(this.booking.value * 0.5);
    this.booking.amount = this.amount4;
  }

  setAmount(value: string) {
    switch (value) {
      case "1":
        this.booking.amount = this.amount1;
        break;
      case "2":
        this.booking.amount = this.amount2;
        break;
      case "3":
        this.booking.amount = this.amount3;
        break;
      case "4":
        this.booking.amount = this.amount4;
        break;
    }
    console.log(this.booking.amount);
  }

  changeView() {
    this.bookingService.setBooking(this.booking);
    this.router.navigate(['/booking/summary'], { relativeTo: this.route });
  }
}
