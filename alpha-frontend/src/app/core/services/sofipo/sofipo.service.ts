import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, map } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface SofipoItemDto {
  id: number;
  name: string;
}

export interface SimulationRequestDto {
  institutionId: number;
  investmentAmount: number;
  months: number;
}

export interface MonthlyDetailDto {
  month: number;
  startingBalance: number;
  yield: number;
  endingBalance: number;
}

export interface SimulationResponseDto {
  monthlyBreakdowns: MonthlyDetailDto[];
  totalEarned: number;
}

export interface PortfolioSimulationResult {
  institutionId: number;
  institutionName: string;
  initialAmount: number;
  totalEarned: number;
  finalBalance: number;
  monthlyBreakdowns: MonthlyDetailDto[];
}

@Injectable({
  providedIn: 'root'
})
export class SofipoService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/sofipos`;

  public institutions = signal<SofipoItemDto[]>([]);

  public getInstitutions(): Observable<SofipoItemDto[]> {
    return this.http.get<SofipoItemDto[]>(`${this.apiUrl}`);
  }

  public loadInstitutions(): void {
    this.getInstitutions().subscribe(data => this.institutions.set(data));
  }

  public simulate(request: SimulationRequestDto): Observable<SimulationResponseDto> {
    return this.http.post<SimulationResponseDto>(`${this.apiUrl}/simulate`, request);
  }

  public simulatePortfolio(investments: { institutionId: number; amount: number }[], months: number): Observable<PortfolioSimulationResult[]> {
    const requests = investments.map(inv => {
      const request: SimulationRequestDto = {
        institutionId: inv.institutionId,
        investmentAmount: inv.amount,
        months: months
      };
      return this.simulate(request).pipe(
        map(response => {
          const institution = this.institutions().find(i => i.id === inv.institutionId);
          return {
            institutionId: inv.institutionId,
            institutionName: institution?.name || 'Unknown',
            initialAmount: inv.amount,
            totalEarned: response.totalEarned,
            finalBalance: inv.amount + response.totalEarned,
            monthlyBreakdowns: response.monthlyBreakdowns
          } as PortfolioSimulationResult;
        })
      );
    });

    return forkJoin(requests);
  }
}
