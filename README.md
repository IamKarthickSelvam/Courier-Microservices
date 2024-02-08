# Courier Microservices

A simple courier booking and tracking application which provides end-to-end functionality from booking orders, payment portal for booked orders and tracking booked and paid orders with a detailed timeline of courier stops. 

The services are all written in C# using .NET Core with ASP.NET Core. Stripe Payments is used to emulate a payment portal. The databases used among these services are SQL Server, SQLite and MongoDB. The web app is written in TypeScript using Angular. All the services are containerized using Linux based Docker and orchestrated in a Kubernetes cluster.

## Tech Stack
.NET 8.0 | Angular v16 | Docker | Kubernetes | RabbitMQ | Stripe | SQL Server | SQLite | MongoDB | Nginx Ingress | Entity Framework Core | gRPC | Automapper | Bootstrap

## Solution Architecture
![Solution Diagram](https://github.com/IamKarthickSelvam/Courier-Microservices/blob/master/Courier%20Microservices.png)

## Working Demo
https://github.com/IamKarthickSelvam/Courier-Microservices/assets/102350733/89fea035-f3c7-41a3-bf2b-b0abc10bfa71

## Application Overview
Users can make a booking from their origin pincode of their choice and their destination pincode along with few other parameters. After summarising the order, the users are redirected to the payment screen. Stripe Payments was used as the payment portal and upon the status of the payment, the users are redirected to Tracking screen where they can track their order based on the consignment ID (Booking ID) provided at the end of payment.

The services follow Event-driven architecture, they are either producing events or consuming events. The Booking is made initially and the bookingId serves are the primary key for all services to update and track the order. The services publish events to a RabbitMQ message broker and subscribes to events emmitted to the queue.

Since the Frontend wasn't the main focus of this project, the UI is a basic but clean set of forms and pages to fetch booking info, navigate to payment portal and to show the timeline of the courier. The price estimation is slightly determined by the value of the item to be shipped but mostly the calculation is very rudimentary frontend-only function and it serves to feed some amount to the payment service.

## Components of Courier Microservices

### Booking Service
The Booking API is responsible for creating a booking and fetching the pincode, order type to the web app. An Origin pincode and Destination pincode is picked up and additional information like weight, dimensions, value of the shipment and type of item is provided from the web app to the API which creates a booking in DB. SQL Server is used for this service and Entity Framework Core is used as the ORM alongside libraries like Automapper, Grpc tools and RabbitMQ. The service generates a bookingId in the process and sends a gRPC call to Payment service sending the booking object as payload, in return receiving the acknowledgment of receival from Payment service. Upon creation of booking, an event is published from the service to the RabbitMQ message broker for Tracking service to track the order.

### Payment Service
The Payment API is responsible for fetching the booking object and generating a Stripe Payment session based on the amount and the bookingId of the booking. A Stripe session consists of a successUrl and cancelUrl based on the outcome of the payment and a sessionId to track the status of the payment later on. A test stripe account was used for this and the payment portal used operated in 'Test mode'. Upon completion of payment, the web app calls an endpoint from payment service with the stripe sessionId to update the payment status from 'Pending' to 'Done' or 'Failed' based on the status of the payment. An event is published upon updation of the booking status to the RabbitMQ message broker for Tracking service to track the order.

### Tracking Service
The Tracking service simply subscribes to the events from Booking and Payment service. As this service simply depends on one booking object to track and update, MongoDB is used as the NoSQL database with each JSON object as a Booking object with origin and destination pincode, bookingId, weight, content type, etc. The bookingId is used to fetch the booking object for the web app to display the timeline of the courier.

### Courier Web App
The web app makes requests to all the three services. It hosts the home screen and views which interact with Booking service (pincode, details forms, courier price estimation screen, summary screen), Payment service (Stripe launch/load screen, Success and Failure screen) and Tracking service (detailed timeline map of courier's stops). For maintaining state through navigation and refresh, the app uses localStorage and RxJS's BehaviorSubject coupled with observables to store booking data, stripe session object, etc.

### Nginx Ingress / API Gateway / Load Balancer
The Nginx Ingress controller deployment in Kubernetes acts as a Load Balancer, API Gateway for services to interact with each other within a Cluster. Since all our services, message broker and SQL Server are dockerized/containerized and hosted in a K8s node, one Nginx Ingress deployment yaml can create networks within the Node for declared services. The Frontend is deployed using a separate Nginx Ingress yaml file with the angular app (Courier Web App) deployment file.

## Credits
Thanks to the owners and contributors of the below projects I've taken inspiration from:
https://github.com/ImranMA/MicroCouriers, https://github.com/binarythistle/S04E03---.NET-Microservices-Course-, https://github.com/dotnet/eShop
