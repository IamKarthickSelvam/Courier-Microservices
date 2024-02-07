import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingService } from 'src/app/_services/booking.service';

@Component({
  selector: 'app-shipmentdetails',
  templateUrl: './shipmentdetails.component.html',
  styleUrls: ['./shipmentdetails.component.css']
})
export class ShipmentdetailsComponent {
  booking: any;

  // Sample array for Content Type dropdown
  contentTypes: string[] = ['', 'Artificial Jewellery', 'Auto/Machine Parts', 'Books', 'Cables/Wires', 'Camera', 'CD', 'Charger Set', 'Cheque Book', 'Chocolates', 'Cloth Item', 'Computer Peripherals', 'Corporate Gifts', 'Credit / Debit Card', 'Documents', 'Dry Fruit', 'Electronic Item', 'Foot Item', 'Furniture', 'Gadget Cover', 'Headphone', 'Home Appliance', 'Household Goods', 'Laptop', 'LED Lights', 'Luggage', 'Medical Equipments', 'Medicine', 'Mobile', 'Painting / Artwork', 'Passport', 'Pen Drive', 'Plastic Items', 'Promotional Material', 'Seeds', 'Shoes', 'Sim card', 'Sweets', 'Toys', 'Zebra'];

  constructor(
    private bookingService: BookingService,
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
    console.log(this.booking);
    this.bookingService.setBooking(this.booking);
    this.router.navigate(['/booking/amount'], { relativeTo: this.route })
  }
}
