import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'appCurrency',
  standalone: true
})
export class CurrencyPipe implements PipeTransform {
  transform(value: number | string | null | undefined, currencyCode: string = 'USD', display: 'code' | 'symbol' | 'symbol-narrow' = 'symbol', digitsInfo?: string, locale: string = 'en-US'): string {
    if (value === null || value === undefined) return '';
    const numericValue = typeof value === 'string' ? parseFloat(value) : value;
    if (isNaN(numericValue)) return '';

    try {
      return new Intl.NumberFormat(locale, {
        style: 'currency',
        currency: currencyCode,
        currencyDisplay: display === 'code' ? 'code' : 'symbol'
      }).format(numericValue);
    } catch (e) {
      return numericValue.toFixed(2);
    }
  }
}
