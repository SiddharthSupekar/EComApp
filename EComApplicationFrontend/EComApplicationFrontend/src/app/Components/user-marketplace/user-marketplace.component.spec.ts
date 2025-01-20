import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserMarketplaceComponent } from './user-marketplace.component';

describe('UserMarketplaceComponent', () => {
  let component: UserMarketplaceComponent;
  let fixture: ComponentFixture<UserMarketplaceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserMarketplaceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserMarketplaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
