export interface Expenses {
  id: number,
  description: string,
  sum: DoubleRange,
  location: string,
  date: Date,
  currency: string,
  Type: Type
  //type: string
}


export enum Type {
  Food,
  Utilities,
  Transportation,
  Outing,
  Groceries,
  Clothes,
  Electronics,
  Other
}
