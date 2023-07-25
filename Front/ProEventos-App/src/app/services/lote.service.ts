import { Evento } from './../models/Evento';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Lote } from '../models/Lote';
import { environment } from 'src/environments/environment';

@Injectable()
export class LoteService {
  baseURL = environment.apiURL +'/api/Lotes';
  constructor(private http: HttpClient) { }
  public getLotesByEventoId(id: number): Observable<Lote[]> {
    return this.http
      .get<Lote[]>(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }
  public saveLote(eventoId: number, lotes: Lote[]): Observable<Lote[]> {
    return this.http
      .put<Lote[]>(`${this.baseURL}/${eventoId}`, lotes)
      .pipe(take(1));
  }
  public deleteLote(eventoId: number, loteid: number): Observable<any> {
    return this.http
      .delete(`${this.baseURL}/${eventoId}/${loteid}`)
      .pipe(take(1));
  }
}
