import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arraySum',
  standalone: true
})
export class ArraySumPipe implements PipeTransform {
  transform(items: any[], property: string): number {
    if (!items || !property) return 0;
    return items.reduce((sum, item) => sum + (item[property] || 0), 0);
  }
}
