export interface Product {
    id?: string;
    name: string;
    price: number;
    stock: number;
    description: string;
    brand: {
        id?: string;
        brandName: string;
    };
    category: {
        id?: string;
        categoryName: string;
    };
    colors: Color[];
    promotionExcluded?: boolean;
}

export interface Color {
    id?: string;
    colorName: string;
}