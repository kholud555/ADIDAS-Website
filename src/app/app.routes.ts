import { Routes } from '@angular/router';
import { DeliverymanRegistrationComponent } from './components/deliveryman-registration.component';

export const routes: Routes = [
  { path: '', component: DeliverymanRegistrationComponent },
  { path: '**', redirectTo: '' }
];