export interface Booking {
    id: number,
    from?: number;
    to?: number;
    weight?: number;
    contentType?: string;
    amount: number;
}