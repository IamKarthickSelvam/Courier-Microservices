import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Tracking } from 'src/app/_models/tracking';
import { TrackingService } from 'src/app/_services/tracking.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent {
  tracking!: Tracking | null;

  today = new Date();
  bookDate = new Date();
  payDate = new Date();
  dispatchDate = new Date();
  arrivalDate = new Date();
  deliveryDate = new Date();

  recentUpdate: string = 'Loading latest status...';
  bookingState: boolean = false;
  paymentState: boolean = false;
  dispatchedState: boolean = false;
  transitState: boolean = false;
  arrivedState: boolean = false;
  deliveredState: boolean = false;

  paymentIsNull: boolean = false;

  constructor(private trackingService: TrackingService,
    private router: Router,
    private route: ActivatedRoute) {
    this.getDetails();
    // this.formatDate('2024-01-30T14:58:03.579Z')
  }

  getDetails() {
    if (localStorage.getItem('tracking') == null) {
      if (this.tracking == null) {
        this.trackingService.trackingDetails$.subscribe({
          next: response => this.tracking = response
        });
      }
    }
    else {
      this.tracking = JSON.parse(localStorage['tracking']);
    }
    this.bookDate = this.formatDate(this.tracking?.bookingTime?.toString()!);
    this.payDate = this.formatDate(this.tracking?.paymentTime?.toString()!);

    if (this.tracking?.paymentTime == null) {
      this.paymentIsNull = true;
    }

    this.setTrackingHistory();
  }

  setTrackingHistory() {

    if (this.tracking?.paymentStatus == 'P') { // Payment Still Pending
      this.recentUpdate = 'Payment still pending, Please complete payment and come back ;-)';
      this.bookingState = true;
      this.paymentState = false;
      this.dispatchedState = false;
      this.transitState = false;
      this.arrivedState = false;
      this.deliveredState = false;
      return null;
    }

    if (this.tracking?.paymentStatus == 'D') { // Payment Done

      if (this.today == this.payDate) { // Today == PaymentDay
        this.recentUpdate = 'Payment done, Yet to be dispatched';
        this.bookingState = true;
        this.paymentState = true;
        this.dispatchedState = false;
        this.transitState = false;
        this.arrivedState = false;
        this.deliveredState = false;
        return null;
      }

      if (this.today > this.payDate) { // Today > PaymentDate

        if (this.payDate.getDate() + 1 > this.today.getDate()) { // PaymentDay + 1 Days > Today
          this.recentUpdate = 'Shipment Dispatched, Awaiting Transit';
          this.bookingState = true;
          this.paymentState = true;
          this.dispatchedState = true;
          this.transitState = false;
          this.arrivedState = false;
          this.deliveredState = false;
          this.dispatchDate.setUTCDate(this.payDate.getDate() + 1);
          this.dispatchDate.setUTCMonth(this.payDate.getMonth());
        }

        if (this.payDate.getDate() + 2 > this.today.getDate()) { // PaymentDay + 2 Days > Today
          this.recentUpdate = 'Currently in Transit, Bound to Arrive @Hub';
          this.bookingState = true;
          this.paymentState = true;
          this.dispatchedState = true;
          this.transitState = true;
          this.arrivedState = false;
          this.deliveredState = false;
        }

        if (this.payDate.getDate() + 3 > this.today.getDate()) { // PaymentDay + 3 Days > Today
          this.recentUpdate = 'Arrived at you nearest Hub, Will be delivered shortly';
          this.bookingState = true;
          this.paymentState = true;
          this.dispatchedState = true;
          this.transitState = true;
          this.arrivedState = true;
          this.deliveredState = false;
          this.arrivalDate.setUTCDate(this.payDate.getDate() + 1);
          this.arrivalDate.setUTCMonth(this.payDate.getMonth());
        }

        if (this.payDate.getDate() + 4 > this.today.getDate()) { // PaymentDay + 4 Days > Today
          this.recentUpdate = 'Shipment Delivered, Your Feedback would be greatly appreciated';
          this.bookingState = true;
          this.paymentState = true;
          this.dispatchedState = true;
          this.transitState = true;
          this.arrivedState = true;
          this.deliveredState = true;
          this.deliveryDate.setUTCDate(this.payDate.getDate() + 1);
          this.deliveryDate.setUTCMonth(this.payDate.getMonth());
        }
        return null;
      }

      else { //Today < PaymentDay
        this.recentUpdate = "Something's not right here, the date of Payment's some date from the future!!! Welcome Time traveller XD";
        this.bookingState = true;
        this.paymentState = true;
        this.dispatchedState = true;
        this.transitState = false;
        this.arrivedState = false;
        this.deliveredState = false;
        return null;
      }
    }

    return null;
  }

  formatDate(dateTime: string) {
    return new Date(dateTime);
  }
}