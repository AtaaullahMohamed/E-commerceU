import { PaymentPipe } from './paymentCard.pipe';

describe('PaymentPipe', () => {
  it('create an instance', () => {
    const pipe = new PaymentPipe();
    expect(pipe).toBeTruthy();
  });
});
