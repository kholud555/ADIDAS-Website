import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { DeliverymanRegistrationComponent } from './components/deliveryman-registration.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, DeliverymanRegistrationComponent],
  template: `
    <div class="app-container">
      <app-deliveryman-registration></app-deliveryman-registration>
    </div>
  `,
  styles: [`
    .app-container {
      min-height: 100vh;
      background-color: var(--body-color);
    }
  `]
})
export class AppComponent {
  title = 'delivery-app';
}