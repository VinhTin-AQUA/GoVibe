import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GovibeCore } from './govibe-core';

describe('GovibeCore', () => {
  let component: GovibeCore;
  let fixture: ComponentFixture<GovibeCore>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GovibeCore]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GovibeCore);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
