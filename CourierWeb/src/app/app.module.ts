import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PincodeComponent } from './booking/pincode/pincode.component';
import { ShipmentdetailsComponent } from './booking/shipmentdetails/shipmentdetails.component';
import { TrackingComponent } from './tracking/tracking.component';
import { HomeComponent } from './home/home.component';
import { BookingService } from './_services/booking.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AmountComponent } from './booking/amount/amount.component';
import { SummaryComponent } from './booking/summary/summary.component';
import { PaymentService } from './_services/payment.service';
import { LoadingComponent } from './payment/loading/loading.component';
import { SuccessComponent } from './payment/success/success.component';
import { FailedComponent } from './payment/failed/failed.component';
import { TrackingService } from './_services/tracking.service';
import { HistoryComponent } from './tracking/history/history.component';

@NgModule({
  declarations: [
    AppComponent,
    PincodeComponent,
    ShipmentdetailsComponent,
    TrackingComponent,
    HomeComponent,
    AmountComponent,
    SummaryComponent,
    LoadingComponent,
    SuccessComponent,
    FailedComponent,
    HistoryComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    BookingService,
    PaymentService,
    TrackingService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
