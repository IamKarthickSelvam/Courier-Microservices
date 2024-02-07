export interface Tracking {
    id: string,
    bookingid: number,
    from: number,
    to: number,
    bookingTime: string,
    paymentTime?: string,
    paymentStatus: string
}