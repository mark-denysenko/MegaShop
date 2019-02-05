import { Component, OnInit } from '@angular/core';
import { Product } from '../../models/product';
import { HttpClient } from '@angular/common/http';
import { ApiResources } from '../../api-resources'
import { map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.css']
})
export class ProductsListComponent implements OnInit {

  public products: Product[] = [];

  constructor(private httpClient: HttpClient) { }

  ngOnInit() {
  	this.httpClient.get<Product[]>(ApiResources.Products).pipe(tap(response => this.products = response)).subscribe();
  }
}
