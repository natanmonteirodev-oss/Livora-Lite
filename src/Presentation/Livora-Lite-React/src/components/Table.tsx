import React from 'react';
import './Table.css';

export interface TableColumn<T> {
  key: keyof T;
  label: string;
  width?: string;
  render?: (value: any, row: T) => React.ReactNode;
}

interface TableProps<T> {
  columns: TableColumn<T>[];
  data: T[];
  loading?: boolean;
  onRowClick?: (row: T) => void;
  actions?: (row: T) => React.ReactNode;
}

export function Table<T>({ 
  columns, 
  data, 
  loading, 
  onRowClick,
  actions 
}: TableProps<T>) {
  if (loading) {
    return (
      <div className="table-loading">
        <div className="loading"></div>
      </div>
    );
  }

  if (data.length === 0) {
    return (
      <div className="table-empty">
        <p>Nenhum dados encontrado</p>
      </div>
    );
  }

  return (
    <div className="table-wrapper">
      <table className="table">
        <thead>
          <tr>
            {columns.map((col) => (
              <th key={String(col.key)} style={{ width: col.width }}>
                {col.label}
              </th>
            ))}
            {actions && <th style={{ width: '120px' }}>Ações</th>}
          </tr>
        </thead>
        <tbody>
          {data.map((row, idx) => (
            <tr 
              key={idx}
              onClick={() => onRowClick?.(row)}
              className={onRowClick ? 'clickable' : ''}
            >
              {columns.map((col) => (
                <td key={String(col.key)}>
                  {col.render ? col.render(row[col.key], row) : String(row[col.key] || '-')}
                </td>
              ))}
              {actions && (
                <td className="actions-cell">
                  {actions(row)}
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
