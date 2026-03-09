import { Component } from '@angular/core';
import { ProjectionDashboard } from './features/projections/projection-dashboard/projection-dashboard';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ProjectionDashboard],
  templateUrl: './app.html' // <- Referencia arreglada
})
export class App { // <- Nombre de clase arreglado para que main.ts compile
}