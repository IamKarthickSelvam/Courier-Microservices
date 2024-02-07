import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TrackingService } from '../_services/tracking.service';

@Component({
  selector: 'app-tracking',
  templateUrl: './tracking.component.html',
  styleUrls: ['./tracking.component.css']
})
export class TrackingComponent {
  tracking: any;
  bookingId: number | undefined;

  constructor(private trackingService: TrackingService,
    private router: Router,
    private route: ActivatedRoute) { }

  changeView() {
    console.log(this.bookingId);
    this.trackingService.getBooking(this.bookingId).subscribe({
      next: response => {
        this.tracking = response;
        this.trackingService.setTrackingData(this.tracking);
        this.router.navigate(['/tracking/history'], { relativeTo: this.route });
      },
      error: error => console.log(error),
      complete: () => console.log(this.tracking)
    });
  }

}
