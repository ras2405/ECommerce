import { Product } from "./product"
import { Promotion } from "./promotion"

export enum PaymentType {
    Visa = 0,
    MasterCard = 1,
    Santander = 2,
    Itau = 3,
    BBVA = 4,
    Paypal = 5,
    Paganza = 6
}

export interface Purchase {
    id: string,
    userId: string,
    products: Product[],
    promotion: Promotion,
    date: string,
    paymentMethod: PaymentType,
    message: string
}