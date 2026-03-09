import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SofipoService, PortfolioSimulationResult } from '../../../core/services/sofipo/sofipo.service';
import { ArraySumPipe } from '../../../shared/pipes/array-sum.pipe';

@Component({
  selector: 'app-sofipo-simulator',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, ArraySumPipe],
  templateUrl: './sofipo-simulator.component.html',
  styleUrl: './sofipo-simulator.component.scss'
})
export class SofipoSimulatorComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  public readonly sofipoService = inject(SofipoService);

  public results = signal<PortfolioSimulationResult[]>([]);
  public isLoading = signal<boolean>(false);
  public error = signal<string | null>(null);

  public form: FormGroup;

  constructor() {
    this.form = this.fb.group({
      months: [12, [Validators.required, Validators.min(1), Validators.max(360)]],
      investments: this.fb.array([this.createInvestmentRow()])
    });
  }

  get investments(): FormArray {
    return this.form.get('investments') as FormArray;
  }

  ngOnInit(): void {
    this.sofipoService.loadInstitutions();
  }

  private createInvestmentRow(): FormGroup {
    return this.fb.group({
      institutionId: [null as number | null, Validators.required],
      amount: [10000, [Validators.required, Validators.min(1)]]
    });
  }

  public addInvestmentRow(): void {
    this.investments.push(this.createInvestmentRow());
  }

  public removeInvestmentRow(index: number): void {
    if (this.investments.length > 1) {
      this.investments.removeAt(index);
    }
  }

  public simulate(): void {
    if (this.form.invalid) return;

    this.isLoading.set(true);
    this.error.set(null);
    this.results.set([]);

    const investments = this.investments.controls.map(control => ({
      institutionId: control.value.institutionId,
      amount: control.value.amount
    }));

    const months = this.form.value.months;

    this.sofipoService.simulatePortfolio(investments, months).subscribe({
      next: (results) => {
        this.results.set(results);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Error al realizar la simulación');
        this.isLoading.set(false);
      }
    });
  }
}
