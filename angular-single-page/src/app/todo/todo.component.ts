// src/app/todo/todo.component.ts
import { Component, OnInit } from '@angular/core';
import { Todo } from './todo.model';
import { TodoService } from './todo.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-todo',
  templateUrl: './todo.component.html',
  styleUrl: './todo.component.scss',
  imports: [FormsModule, CommonModule],
  standalone: true
})
export class TodoComponent implements OnInit {
  todos: Todo[] = [];
  newTask: string = '';
  isError: boolean = false;

  constructor(private todoService: TodoService) {}

  ngOnInit(): void {
    this.fetchTodos();
  }

  fetchTodos() {
    this.todoService.getTodos().subscribe({
      next: (todos: Todo[]) => {
        this.isError = false;
        this.todos = todos;
      },
      error: (err) => {
        this.isError = true;
      }
    });
  }
}