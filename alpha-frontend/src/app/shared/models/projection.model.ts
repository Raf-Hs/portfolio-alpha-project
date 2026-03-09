export interface ProjectionRequest {
  initialCapital: number;
  annualInterestRate: number;
  termInMonths: number;
}

export interface ProjectionRow {
  period: number;
  startingBalance: number;
  interestEarned: number;
  endingBalance: number;
}