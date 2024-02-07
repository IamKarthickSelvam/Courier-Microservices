import { Component } from '@angular/core';
import { Form, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Booking } from 'src/app/_models/booking';
import { BookingService } from 'src/app/_services/booking.service';

@Component({
  selector: 'app-pincode',
  templateUrl: './pincode.component.html',
  styleUrls: ['./pincode.component.css']
})
export class PincodeComponent {
  booking: any = {};
  formattedPincodes: { code: number, displayText: string }[] = [];

  pincodeForm = FormGroup;

  constructor(
    private bookingService: BookingService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder) {
    this.getPincodes();
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
    console.log(this.booking);

  }

  getPincodes() {
    this.bookingService.getPincodes().subscribe({
      next: response => {
        this.formattedPincodes = response.map((city: { pincodeList: any; code: any; name: any; }) => {
          return {
            code: city.code,
            displayText: `${city.code} - ${city.name}`
          };
        })
      },
      error: error => console.log(error),
      complete: () => console.log('Successfully received Pincodes')
    });
  }

  changeView() {
    this.bookingService.setBooking(this.booking);
    this.router.navigate(['/booking/shipment-details'], { relativeTo: this.route })
  }

}
