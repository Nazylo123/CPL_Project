import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CartComponent } from "./carts/cart/cart.component";
import { NavbarComponent } from "./navbar/navbar.component";
import { FooterComponent } from './footer/footer.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CartComponent, NavbarComponent,FooterComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'UI';
}
