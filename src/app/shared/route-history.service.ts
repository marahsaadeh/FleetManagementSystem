import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { throwError } from 'rxjs';
import { map, tap,catchError } from 'rxjs/operators';
import { RouteHistory } from './route-history.model';


@Injectable({
  providedIn: 'root'
})
export class RouteHistoryService {
  url:string=environment.apiBaseUrl+'/RouteHistory'
  private apiUrl =  'https://localhost:7062/api/RouteHistory';

 // list:RouteHistory[]=[];
  
  formDate:RouteHistory=new RouteHistory()
  formSubmittes:boolean=false;
  public fullData: any = null; //Postman
  private listSource = new BehaviorSubject<RouteHistory[]>([]);
  list = this.listSource.asObservable();
  public rowCount: number = 0;
  constructor(private http: HttpClient) {}
  

  getRouteHistories(): Observable<any[]> {
    return this.http.get<{ DicOfDT: { RouteHistories: any[] } }>(
      `${this.apiUrl}/GetAll`
    ).pipe(
      map(response => {
        this.fullData = response.DicOfDT.RouteHistories;
        return response.DicOfDT.RouteHistories;
      }),
      tap(routeHistories => {
        this.rowCount = routeHistories.length; // تعيين rowCount إلى عدد سجلات الطرق التاريخية المسترجعة
        console.log('عدد سجلات الطرق : ', this.rowCount);
      }),
      catchError(error => {
        console.error('Error fetching Route Histories:', error);
        return of([]); // إرجاع مصفوفة فارغة في حالة الخطأ لضمان عدم انقطاع الاشتراك
      })
    );
  }
   


  addRouteHistory(routeHistory: RouteHistory): Observable<RouteHistory> {
      const headers = new HttpHeaders({'Content-Type': 'application/json'});
      return this.http.post<RouteHistory>(`${this.apiUrl}/Add`, JSON.stringify(routeHistory), { headers }).pipe(
          tap((newRouteHistory: RouteHistory) => {
            const updatedHistories = [...this.listSource.value, newRouteHistory];
        this.listSource.next(updatedHistories);
      }),
          catchError(this.handleError)
      );
  }
  


  updateRouteHistory(routeHistory: any): Observable<any> {
    const headers = new HttpHeaders({'Content-Type': 'application/json'});
    return this.http.put(`${this.apiUrl}/${routeHistory.VehicleID}`, JSON.stringify(routeHistory), { headers })
      .pipe(
        catchError(this.handleError)
      );
  }
  private handleError(error: any) {
    console.error('An error occurred:', error.message || error);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
