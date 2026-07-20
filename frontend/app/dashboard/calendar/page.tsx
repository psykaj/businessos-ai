"use client";

import { useQuery } from "@tanstack/react-query";
import { crmService } from "@/lib/crm-service";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { ChevronLeft, ChevronRight, Calendar as CalendarIcon } from "lucide-react";
import { useState } from "react";

export default function CalendarPage() {
  const [currentDate, setCurrentDate] = useState(new Date());

  const { data: tasks, isLoading } = useQuery({
    queryKey: ['crm-tasks'],
    queryFn: () => crmService.getTasks()
  });

  const nextMonth = () => {
    setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 1));
  };

  const prevMonth = () => {
    setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 1));
  };

  const getDaysInMonth = (year: number, month: number) => {
    return new Date(year, month + 1, 0).getDate();
  };

  const getFirstDayOfMonth = (year: number, month: number) => {
    return new Date(year, month, 1).getDay();
  };

  const year = currentDate.getFullYear();
  const month = currentDate.getMonth();
  const daysInMonth = getDaysInMonth(year, month);
  const firstDay = getFirstDayOfMonth(year, month);
  
  const monthName = currentDate.toLocaleString('default', { month: 'long' });

  // Filter tasks that have a due date in this month
  const getTasksForDay = (day: number) => {
    if (!tasks) return [];
    return tasks.filter(task => {
      if (!task.dueDate) return false;
      const taskDate = new Date(task.dueDate);
      return taskDate.getDate() === day && 
             taskDate.getMonth() === month && 
             taskDate.getFullYear() === year;
    });
  };

  return (
    <div className="p-8 space-y-6 h-full flex flex-col">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Calendar</h1>
          <p className="text-muted-foreground">View your upcoming tasks and meetings.</p>
        </div>
        <div className="flex items-center gap-2">
          <Button variant="outline" size="icon" onClick={prevMonth}>
            <ChevronLeft className="h-4 w-4" />
          </Button>
          <div className="w-40 text-center font-semibold text-lg">
            {monthName} {year}
          </div>
          <Button variant="outline" size="icon" onClick={nextMonth}>
            <ChevronRight className="h-4 w-4" />
          </Button>
        </div>
      </div>

      <Card className="flex-1 flex flex-col min-h-[600px]">
        <CardContent className="p-6 flex-1 flex flex-col h-full">
          {isLoading ? (
            <Skeleton className="h-full w-full" />
          ) : (
            <div className="flex-1 grid grid-cols-7 gap-px bg-border rounded-lg overflow-hidden border">
              {['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].map((day) => (
                <div key={day} className="bg-muted/50 p-2 text-center text-sm font-semibold text-muted-foreground">
                  {day}
                </div>
              ))}
              
              {/* Empty cells before the 1st */}
              {Array.from({ length: firstDay }).map((_, i) => (
                <div key={`empty-${i}`} className="bg-background min-h-[100px] p-2 opacity-50" />
              ))}
              
              {/* Actual days */}
              {Array.from({ length: daysInMonth }).map((_, i) => {
                const day = i + 1;
                const isToday = day === new Date().getDate() && 
                               month === new Date().getMonth() && 
                               year === new Date().getFullYear();
                
                const dayTasks = getTasksForDay(day);

                return (
                  <div 
                    key={day} 
                    className={`bg-background min-h-[100px] p-2 border-t hover:bg-muted/20 transition-colors ${
                      isToday ? 'bg-primary/5' : ''
                    }`}
                  >
                    <div className={`text-sm font-medium w-7 h-7 flex items-center justify-center rounded-full mb-1 ${
                      isToday ? 'bg-primary text-primary-foreground' : 'text-muted-foreground'
                    }`}>
                      {day}
                    </div>
                    
                    <div className="space-y-1">
                      {dayTasks.map(task => (
                        <div 
                          key={task.id} 
                          className="text-xs truncate bg-blue-50 text-blue-700 border border-blue-200 px-1.5 py-1 rounded"
                          title={task.title}
                        >
                          {task.title}
                        </div>
                      ))}
                    </div>
                  </div>
                );
              })}
              
              {/* Empty cells after the last day */}
              {Array.from({ length: 42 - (firstDay + daysInMonth) }).map((_, i) => (
                <div key={`empty-end-${i}`} className="bg-background min-h-[100px] p-2 opacity-50 border-t" />
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
