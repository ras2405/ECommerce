export enum PromotionType {
    twenty = 0,
    total = 1,
    threeXtwo = 2,
    threeXone = 3
}
export interface Promotion {
    id: string,
    amount: number,
    type: number,
    message: string
}