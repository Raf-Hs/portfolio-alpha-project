import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProjectionRequest, ProjectionRow } from '../../../shared/models/projection.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProjectionService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Projections`;

  public calculateCompoundInterest(request: ProjectionRequest): Observable<ProjectionRow[]> {
    return this.http.post<ProjectionRow[]>(`${this.apiUrl}/compound-interest`, request);
  }
}