"use client";

import React from "react";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

interface ColorPickerProps {
  value: string;
  onChange: (value: string) => void;
  label?: string;
}

export function ColorPicker({ value, onChange, label }: ColorPickerProps) {
  let r = 0, g = 0, b = 0;
  let hexValue = value;
  if (hexValue && /^#[0-9A-F]{3}$/i.test(hexValue)) {
    hexValue = "#" + hexValue[1] + hexValue[1] + hexValue[2] + hexValue[2] + hexValue[3] + hexValue[3];
  }
  if (hexValue && /^#[0-9A-F]{6}$/i.test(hexValue)) {
    r = parseInt(hexValue.slice(1, 3), 16);
    g = parseInt(hexValue.slice(3, 5), 16);
    b = parseInt(hexValue.slice(5, 7), 16);
  }

  const handleRgbChange = (color: 'r' | 'g' | 'b', val: string) => {
    let num = parseInt(val, 10);
    if (isNaN(num)) num = 0;
    if (num > 255) num = 255;
    if (num < 0) num = 0;

    const newRgb = { r, g, b, [color]: num };

    // Convert new RGB back to HEX and trigger onChange
    const hex = "#" + (
      (1 << 24) + 
      (newRgb.r << 16) + 
      (newRgb.g << 8) + 
      newRgb.b
    ).toString(16).slice(1).toUpperCase();
    
    onChange(hex);
  };

  return (
    <div className="space-y-3 border rounded-md p-4 bg-card shadow-sm w-full">
      {label && <Label className="font-semibold text-sm">{label}</Label>}
      
      <div className="flex gap-4 items-start">
        <div className="flex flex-col items-center gap-1">
          <Input 
            type="color" 
            className="w-14 p-1 h-14 cursor-pointer" 
            value={value.length === 7 ? value : "#000000"} 
            onChange={(e) => onChange(e.target.value.toUpperCase())}
          />
        </div>

        <div className="flex-1 space-y-3">
          <div className="flex items-center gap-2">
            <Label className="w-8 text-xs text-muted-foreground">HEX</Label>
            <Input 
              value={value} 
              onChange={(e) => onChange(e.target.value)} 
              className="font-mono text-xs h-8 uppercase"
              placeholder="#FFFFFF"
            />
          </div>

          <div className="flex items-center gap-2">
             <Label className="w-8 text-xs text-muted-foreground">RGB</Label>
             <div className="flex gap-1 w-full">
                <Input 
                  type="number" 
                  min="0" max="255" 
                  value={r.toString()} 
                  onChange={(e) => handleRgbChange('r', e.target.value)}
                  className="font-mono text-xs h-8 px-2 flex-1 min-w-0"
                  title="Red (0-255)"
                  placeholder="R"
                />
                <Input 
                  type="number" 
                  min="0" max="255" 
                  value={g.toString()} 
                  onChange={(e) => handleRgbChange('g', e.target.value)}
                  className="font-mono text-xs h-8 px-2 flex-1 min-w-0"
                  title="Green (0-255)"
                  placeholder="G"
                />
                <Input 
                  type="number" 
                  min="0" max="255" 
                  value={b.toString()} 
                  onChange={(e) => handleRgbChange('b', e.target.value)}
                  className="font-mono text-xs h-8 px-2 flex-1 min-w-0"
                  title="Blue (0-255)"
                  placeholder="B"
                />
             </div>
          </div>
        </div>
      </div>
    </div>
  );
}
