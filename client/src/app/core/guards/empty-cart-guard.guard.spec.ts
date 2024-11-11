import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { emptyCartGuardGuard } from './empty-cart-guard.guard';

describe('emptyCartGuardGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => emptyCartGuardGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
