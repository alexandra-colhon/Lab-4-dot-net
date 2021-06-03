export interface Expenses {
  id: number,
  description: string,
  sum: DoubleRange,
  location: string,
  date: Date,
  currency: string,
  // Type Type
  type: string
}
