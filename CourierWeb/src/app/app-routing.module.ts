import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PincodeComponent } from './booking/pincode/pincode.component';
import { ShipmentdetailsComponent } from './booking/shipmentdetails/shipmentdetails.component';
import { TrackingComponent } from './tracking/tracking.component';
import { HomeComponent } from './home/home.component';
import { AmountComponent } from './booking/amount/amount.component';
import { SummaryComponent } from './booking/summary/summary.component';
import { LoadingComponent } from './payment/loading/loading.component';
import { SuccessComponent } from './payment/success/success.component';
import { FailedComponent } from './payment/failed/failed.component';
import { HistoryComponent } from './tracking/history/history.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'booking/pincode', component: PincodeComponent },
  { path: 'booking/shipment-details', component: ShipmentdetailsComponent },
  { path: 'booking/amount', component: AmountComponent },
  { path: 'booking/summary', component: SummaryComponent },
  { path: 'payment/loading', component: LoadingComponent },
  { path: 'payment/success', component: SuccessComponent },
  { path: 'payment/failed', component: FailedComponent },
  { path: 'tracking', component: TrackingComponent },
  { path: 'tracking/history', component: HistoryComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
