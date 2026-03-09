import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectionDashboard } from './projection-dashboard';

describe('ProjectionDashboard', () => {
  let component: ProjectionDashboard;
  let fixture: ComponentFixture<ProjectionDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectionDashboard],
    }).compileComponents();

    fixture = TestBed.createComponent(ProjectionDashboard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
