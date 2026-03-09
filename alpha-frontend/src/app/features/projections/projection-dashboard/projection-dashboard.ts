import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common'; 
import { ProjectionService } from '../../../core/services/projection/projection.service';
import { ProjectionRow } from '../../../shared/models/projection.model';

@Component({
  selector: 'app-projection-dashboard',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule], 
  templateUrl: './projection-dashboard.html',
  styleUrl: './projection-dashboard.scss' // <- Referencia arreglada
})
export class ProjectionDashboard { // <- Nombre de clase arreglado
  private readonly fb = inject(FormBuilder);
  private readonly projectionService = inject(ProjectionService);

  public projectionData = signal<ProjectionRow[]>([]);
  public isLoading = signal<boolean>(false);
  public error = signal<string | null>(null);

  public form = this.fb.group({
    initialCapital: [10000, [Validators.required, Validators.min(1)]],
    annualInterestRate: [15, [Validators.required, Validators.min(0)]],
    termInMonths: [12, [Validators.required, Validators.min(1), Validators.max(360)]]
  });

  public calculate(): void {
    if (this.form.invalid) return;

    this.isLoading.set(true);
    this.error.set(null);

    const request = this.form.getRawValue() as any;

    this.projectionService.calculateCompoundInterest(request).subscribe({
      next: (response) => {
        this.projectionData.set(response);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.error.set('Error al conectar con .NET.');
        this.isLoading.set(false);
      }
    });
  }
}