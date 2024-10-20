import { TestBed } from '@angular/core/testing';

import { AddrssService } from './addrss.service';

describe('AddrssService', () => {
  let service: AddrssService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AddrssService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
