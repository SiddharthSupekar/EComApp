import { TestBed } from '@angular/core/testing';

import { CountrystateService } from './countrystate.service';

describe('CountrystateService', () => {
  let service: CountrystateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CountrystateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
