import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { SofipoService, PortfolioSimulationResult, SofipoItemDto, TieredInterestRule } from '../../../core/services/sofipo/sofipo.service';
import { ArraySumPipe } from '../../../shared/pipes/array-sum.pipe';

@Component({
  selector: 'app-sofipo-simulator',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, BaseChartDirective, ArraySumPipe],
  templateUrl: './sofipo-simulator.component.html',
  styleUrl: './sofipo-simulator.component.scss'
})
export class SofipoSimulatorComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  public readonly sofipoService = inject(SofipoService);

  public results = signal<PortfolioSimulationResult[]>([]);
  public isLoading = signal<boolean>(false);
  public error = signal<string | null>(null);
  public isEnglish = signal<boolean>(false);

  public selectedRules = signal<TieredInterestRule | null>(null);

  public form: FormGroup;

  public doughnutChartData = signal<ChartData<'doughnut'>>({
    labels: [],
    datasets: []
  });

  public doughnutChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'bottom',
        labels: {
          color: '#9ca3af',
          padding: 20,
          font: { size: 12 }
        }
      }
    }
  };

  constructor() {
    this.form = this.fb.group({
      months: [12, [Validators.required, Validators.min(1), Validators.max(360)]],
      investments: this.fb.array([this.createInvestmentRow()])
    });

    this.form.get('investments')?.valueChanges.subscribe(values => {
      const firstWithValue = values.find((v: { institutionId: number | null }) => v.institutionId !== null);
      if (firstWithValue && firstWithValue.institutionId) {
        this.loadInstitutionRules(firstWithValue.institutionId);
      } else {
        this.selectedRules.set(null);
      }
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

  private loadInstitutionRules(institutionId: number): void {
    const institutions = this.sofipoService.institutions();
    const mockRules: TieredInterestRule[] = [
      { id: 1, institutionId: 1, limitAmount: 25000, primaryRate: 13.00, fallbackRate: 7.00 },
      { id: 2, institutionId: 2, limitAmount: 100000, primaryRate: 13.00, fallbackRate: 0.00 },
      { id: 3, institutionId: 3, limitAmount: 10000, primaryRate: 15.00, fallbackRate: 0.00 }
    ];
    const index = institutionId - 1;
    if (index >= 0 && index < mockRules.length) {
      this.selectedRules.set(mockRules[index]);
    }
  }

  public toggleLanguage(): void {
    this.isEnglish.update(v => !v);
  }

  public t(key: string): string {
    const translations: Record<string, Record<string, string>> = {
      title: { es: 'Simulador de Portafolio SOFIPO', en: 'SOF Portfolio Simulator' },
      configure: { es: 'Configurar Portafolio', en: 'Configure Portfolio' },
      months: { es: 'Plazo (meses)', en: 'Period (months)' },
      investments: { es: 'Inversiones', en: 'Investments' },
      selectSofipo: { es: 'Selecciona SOFIPO', en: 'Select SOFIPO' },
      amount: { es: 'Monto', en: 'Amount' },
      addSofipo: { es: '+ Agregar SOFIPO', en: '+ Add SOFIPO' },
      simulate: { es: 'Simular Portafolio', en: 'Simulate Portfolio' },
      calculating: { es: 'Calculando...', en: 'Calculating...' },
      distribution: { es: 'Distribución del Capital', en: 'Capital Distribution' },
      results: { es: 'Resultados del Portafolio', en: 'Portfolio Results' },
      simulateToSee: { es: 'Simula para ver la distribución', en: 'Simulate to see distribution' },
      addAndSimulate: { es: 'Agrega inversiones y simula tu portafolio', en: 'Add investments and simulate your portfolio' },
      capital: { es: 'Capital', en: 'Capital' },
      yield: { es: 'Interés', en: 'Yield' },
      final: { es: 'Final', en: 'Final' },
      totalCapital: { es: 'Capital Total', en: 'Total Capital' },
      totalYield: { es: 'Interés Total', en: 'Total Yield' },
      totalFinal: { es: 'Total Final', en: 'Total Final' },
      technicalSheet: { es: 'Ficha Técnica', en: 'Technical Sheet' },
      baseRate: { es: 'Tasa Base', en: 'Base Rate' },
      preferentialRate: { es: 'Tasa Preferencial', en: 'Preferential Rate' },
      limitAmount: { es: 'Monto Tope', en: 'Limit Amount' },
      monthsLabel: { es: 'meses', en: 'months' }
    };
    return translations[key]?.[this.isEnglish() ? 'en' : 'es'] || key;
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
        this.updateChart(results);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set(this.isEnglish() ? 'Error al realizar la simulación' : 'Error performing simulation');
        this.isLoading.set(false);
      }
    });
  }

  private updateChart(results: PortfolioSimulationResult[]): void {
    const labels = results.map(r => r.institutionName);
    const data = results.map(r => r.initialAmount);
    const colors = ['#10b981', '#3b82f6', '#f59e0b', '#ef4444', '#8b5cf6'];

    this.doughnutChartData.set({
      labels: labels,
      datasets: [{
        data: data,
        backgroundColor: colors.slice(0, results.length),
        borderColor: '#1f2937',
        borderWidth: 2
      }]
    });
  }
}
