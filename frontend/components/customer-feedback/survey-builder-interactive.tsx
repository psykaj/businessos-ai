"use client";

import React, { useState } from "react";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { Badge } from "@/components/ui/badge";
import { Plus, ArrowUp, ArrowDown, Trash2, Eye, Play, Pause, Sparkles, CheckCircle2, ListOrdered, FileText, Star, Share2 } from "lucide-react";
import { SurveyCampaignDto, SurveyQuestionDto } from "@/lib/customer-feedback-service";
import { useCreateSurvey, useUpdateSurveyQuestions, useTogglePublishSurvey } from "@/hooks/use-customer-feedback";

interface SurveyBuilderInteractiveProps {
  surveys?: SurveyCampaignDto[];
  isLoading?: boolean;
}

export function SurveyBuilderInteractive({ surveys = [], isLoading }: SurveyBuilderInteractiveProps) {
  const createMutation = useCreateSurvey();
  const updateQuestionsMutation = useUpdateSurveyQuestions();
  const togglePublishMutation = useTogglePublishSurvey();

  // Selected survey for editing
  const [selectedSurveyId, setSelectedSurveyId] = useState<string>(surveys[0]?.id || "srv-201");
  const activeSurvey = surveys.find((s) => s.id === selectedSurveyId) || surveys[0];

  // Local state for question editing
  const [questions, setQuestions] = useState<SurveyQuestionDto[]>(activeSurvey?.questions || []);
  const [newQuestionText, setNewQuestionText] = useState("");
  const [newQuestionType, setNewQuestionType] = useState<"StarRating" | "NpsScale" | "MultipleChoice" | "Text">("StarRating");

  // New Survey Campaign modal state
  const [isNewModalOpen, setIsNewModalOpen] = useState(false);
  const [newTitle, setNewTitle] = useState("");
  const [newDesc, setNewDesc] = useState("");
  const [newType, setNewType] = useState<"CSAT" | "NPS" | "CES">("NPS");
  const [newTarget, setNewTarget] = useState("All Tier-1 Enterprise Accounts ($10k+ ARR)");

  // Live preview modal state
  const [isPreviewOpen, setIsPreviewOpen] = useState(false);

  // Sync questions when changing active survey
  React.useEffect(() => {
    if (activeSurvey) {
      setQuestions(activeSurvey.questions || []);
    }
  }, [selectedSurveyId, activeSurvey?.id]);

  const handleAddQuestion = (e: React.FormEvent) => {
    e.preventDefault();
    if (!newQuestionText.trim() || !activeSurvey) return;
    const nextOrder = questions.length + 1;
    const newQ: SurveyQuestionDto = {
      id: `q-custom-${Date.now()}`,
      questionText: newQuestionText,
      questionType: newQuestionType,
      orderIndex: nextOrder,
      isRequired: true,
      options: newQuestionType === "MultipleChoice" ? ["Highly Satisfied", "Neutral", "Needs Improvement"] : undefined,
    };
    const updated = [...questions, newQ];
    setQuestions(updated);
    setNewQuestionText("");
    updateQuestionsMutation.mutate({ surveyId: activeSurvey.id, questions: updated });
  };

  const handleMoveQuestion = (index: number, direction: "up" | "down") => {
    if (!activeSurvey) return;
    const targetIndex = direction === "up" ? index - 1 : index + 1;
    if (targetIndex < 0 || targetIndex >= questions.length) return;

    const copy = [...questions];
    const temp = copy[index];
    copy[index] = copy[targetIndex];
    copy[targetIndex] = temp;

    // reindex
    copy.forEach((q, i) => (q.orderIndex = i + 1));
    setQuestions(copy);
    updateQuestionsMutation.mutate({ surveyId: activeSurvey.id, questions: copy });
  };

  const handleDeleteQuestion = (id: string) => {
    if (!activeSurvey) return;
    const updated = questions.filter((q) => q.id !== id);
    updated.forEach((q, i) => (q.orderIndex = i + 1));
    setQuestions(updated);
    updateQuestionsMutation.mutate({ surveyId: activeSurvey.id, questions: updated });
  };

  const handleCreateCampaign = () => {
    if (!newTitle.trim()) return;
    createMutation.mutate(
      { title: newTitle, description: newDesc, surveyType: newType, targetAudience: newTarget },
      {
        onSuccess: (created) => {
          setSelectedSurveyId(created.id);
          setIsNewModalOpen(false);
          setNewTitle("");
          setNewDesc("");
        },
      }
    );
  };

  const handleTogglePublish = () => {
    if (!activeSurvey) return;
    togglePublishMutation.mutate({ surveyId: activeSurvey.id, isActive: !activeSurvey.isActive });
  };

  if (isLoading) {
    return <div className="p-12 text-center text-slate-500 animate-pulse">Loading Survey Studio & Campaigns...</div>;
  }

  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
      {/* Left Column: Survey Campaigns List */}
      <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-sm rounded-xl h-fit">
        <CardHeader className="p-5 border-b border-slate-100 dark:border-slate-800 flex flex-row items-center justify-between">
          <div>
            <CardTitle className="text-base font-bold text-slate-900 dark:text-white flex items-center gap-2">
              <ListOrdered className="h-4 w-4 text-indigo-500" />
              Active Survey Campaigns
            </CardTitle>
            <CardDescription className="text-xs text-slate-500 mt-0.5">Select a campaign to edit questions & rules</CardDescription>
          </div>
          <Button
            size="sm"
            onClick={() => setIsNewModalOpen(true)}
            className="bg-indigo-600 hover:bg-indigo-700 text-white text-xs h-8 px-3 rounded-lg shadow font-semibold flex items-center gap-1"
          >
            <Plus className="h-3.5 w-3.5" />
            New
          </Button>
        </CardHeader>

        <CardContent className="p-3 divide-y divide-slate-100 dark:divide-slate-800">
          {surveys.map((survey) => {
            const isSelected = survey.id === (activeSurvey?.id || "");
            return (
              <div
                key={survey.id}
                onClick={() => setSelectedSurveyId(survey.id)}
                className={`p-3.5 rounded-lg cursor-pointer transition-all ${
                  isSelected ? "bg-indigo-50 dark:bg-indigo-950/40 border border-indigo-200 dark:border-indigo-800 shadow-sm" : "hover:bg-slate-50 dark:hover:bg-slate-800/50"
                }`}
              >
                <div className="flex items-center justify-between">
                  <span className="text-sm font-bold text-slate-900 dark:text-white truncate">{survey.title}</span>
                  <Badge className={`text-[10px] uppercase font-bold ${survey.isActive ? "bg-emerald-100 text-emerald-800 dark:bg-emerald-900/50 dark:text-emerald-300" : "bg-slate-100 text-slate-600"}`}>
                    {survey.isActive ? "Active" : "Draft"}
                  </Badge>
                </div>
                <div className="flex items-center justify-between mt-2 text-xs text-slate-500 dark:text-slate-400">
                  <span>Type: <strong className="text-indigo-600 dark:text-indigo-400">{survey.surveyType}</strong></span>
                  <span>{survey.totalResponses} Responses ({survey.completionRate}% completion)</span>
                </div>
              </div>
            );
          })}
        </CardContent>
      </Card>

      {/* Right 2 Columns: Interactive Question Studio */}
      <div className="lg:col-span-2 space-y-6">
        <Card className="bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 shadow-md rounded-xl">
          <CardHeader className="p-6 border-b border-slate-200 dark:border-slate-800 bg-slate-50/40 dark:bg-slate-900/40">
            <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
              <div>
                <div className="flex items-center gap-2">
                  <CardTitle className="text-xl font-bold text-slate-900 dark:text-white">{activeSurvey?.title}</CardTitle>
                  <Badge className="bg-indigo-100 text-indigo-800 dark:bg-indigo-900/40 dark:text-indigo-300 font-mono text-xs">{activeSurvey?.surveyType}</Badge>
                </div>
                <CardDescription className="text-xs text-slate-500 mt-1">{activeSurvey?.description}</CardDescription>
                <div className="mt-2 flex items-center gap-3 text-xs font-semibold text-slate-600 dark:text-slate-400">
                  <span>Target: <strong className="text-indigo-600 dark:text-indigo-400">{activeSurvey?.targetAudience}</strong></span>
                  <span>•</span>
                  <span>Average Score: <strong className="text-emerald-600 dark:text-emerald-400">{activeSurvey?.averageScore} / 5.0</strong></span>
                </div>
              </div>

              <div className="flex items-center gap-2">
                <Button variant="outline" size="sm" onClick={() => setIsPreviewOpen(true)} className="flex items-center gap-1.5 text-xs h-9 font-semibold shadow-sm">
                  <Eye className="h-4 w-4 text-slate-600 dark:text-slate-400" />
                  Preview Widget
                </Button>
                <Button
                  size="sm"
                  onClick={handleTogglePublish}
                  disabled={togglePublishMutation.isPending}
                  className={`flex items-center gap-1.5 text-xs h-9 px-3 font-semibold shadow-md ${
                    activeSurvey?.isActive
                      ? "bg-amber-500 hover:bg-amber-600 text-white"
                      : "bg-emerald-600 hover:bg-emerald-700 text-white"
                  }`}
                >
                  {activeSurvey?.isActive ? (
                    <>
                      <Pause className="h-4 w-4" /> Pause Campaign
                    </>
                  ) : (
                    <>
                      <Play className="h-4 w-4 fill-current" /> Publish Survey
                    </>
                  )}
                </Button>
              </div>
            </div>
          </CardHeader>

          <CardContent className="p-6 space-y-6">
            {/* Add New Question Form */}
            <form onSubmit={handleAddQuestion} className="bg-slate-50 dark:bg-slate-800/50 p-4 rounded-xl border border-slate-200 dark:border-slate-700 space-y-3">
              <h4 className="text-sm font-bold text-slate-800 dark:text-white flex items-center gap-1.5">
                <Plus className="h-4 w-4 text-indigo-500" /> Add Question to Campaign Template
              </h4>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                <div className="md:col-span-2">
                  <Input
                    placeholder="Enter question wording (e.g. How likely are you to recommend us?)..."
                    value={newQuestionText}
                    onChange={(e) => setNewQuestionText(e.target.value)}
                    className="h-10 text-sm bg-white dark:bg-slate-900 border-slate-200 dark:border-slate-700"
                  />
                </div>
                <div>
                  <select
                    value={newQuestionType}
                    onChange={(e) => setNewQuestionType(e.target.value as any)}
                    className="w-full h-10 px-3 bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-700 rounded-lg text-sm font-medium text-slate-800 dark:text-slate-200 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                  >
                    <option value="StarRating">Star Rating (1-5 Stars)</option>
                    <option value="NpsScale">NPS Scale (0-10 Score)</option>
                    <option value="MultipleChoice">Multiple Choice</option>
                    <option value="Text">Free-Form Text Feedback</option>
                  </select>
                </div>
              </div>
              <div className="flex justify-end">
                <Button type="submit" size="sm" className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold text-xs px-4 h-8">
                  Append Question
                </Button>
              </div>
            </form>

            {/* Existing Questions List with Reordering */}
            <div className="space-y-3">
              <h4 className="text-sm font-bold text-slate-800 dark:text-white uppercase tracking-wider text-xs">
                Campaign Questions ({questions.length})
              </h4>
              {questions.length === 0 ? (
                <div className="p-8 text-center text-slate-400 text-sm border border-dashed border-slate-200 dark:border-slate-800 rounded-lg">
                  No questions assigned to this campaign yet. Add one above!
                </div>
              ) : (
                questions.map((q, idx) => (
                  <div
                    key={q.id}
                    className="flex items-center justify-between bg-white dark:bg-slate-800/80 p-4 rounded-xl border border-slate-200 dark:border-slate-700 hover:border-indigo-300 dark:hover:border-indigo-700 transition-all shadow-sm"
                  >
                    <div className="flex items-center gap-3">
                      <span className="flex items-center justify-center h-7 w-7 rounded-full bg-indigo-100 dark:bg-indigo-900/50 text-indigo-700 dark:text-indigo-300 font-bold text-xs">
                        {q.orderIndex}
                      </span>
                      <div>
                        <p className="text-sm font-bold text-slate-900 dark:text-white">{q.questionText}</p>
                        <div className="flex items-center gap-2 mt-1 text-xs text-slate-500 dark:text-slate-400">
                          <Badge variant="outline" className="text-[10px] font-mono bg-slate-50 dark:bg-slate-800">
                            {q.questionType}
                          </Badge>
                          <span>{q.isRequired ? "Mandatory response" : "Optional response"}</span>
                        </div>
                      </div>
                    </div>

                    <div className="flex items-center gap-1">
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => handleMoveQuestion(idx, "up")}
                        disabled={idx === 0}
                        title="Move Up"
                        className="h-8 w-8 text-slate-500 hover:text-slate-900 dark:hover:text-white"
                      >
                        <ArrowUp className="h-4 w-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => handleMoveQuestion(idx, "down")}
                        disabled={idx === questions.length - 1}
                        title="Move Down"
                        className="h-8 w-8 text-slate-500 hover:text-slate-900 dark:hover:text-white"
                      >
                        <ArrowDown className="h-4 w-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => handleDeleteQuestion(q.id)}
                        title="Remove Question"
                        className="h-8 w-8 text-rose-500 hover:bg-rose-50 dark:hover:bg-rose-950/30"
                      >
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                ))
              )}
            </div>
          </CardContent>
        </Card>
      </div>

      {/* New Campaign Creation Modal */}
      <Dialog open={isNewModalOpen} onOpenChange={setIsNewModalOpen}>
        <DialogContent className="max-w-md p-6 bg-white dark:bg-slate-900 rounded-xl border border-slate-200 dark:border-slate-800 shadow-2xl">
          <DialogHeader>
            <DialogTitle className="text-lg font-bold text-slate-900 dark:text-white flex items-center gap-2">
              <Sparkles className="h-5 w-5 text-indigo-500" /> Create Survey Campaign
            </DialogTitle>
            <CardDescription className="text-xs text-slate-500">
              Automated survey pulse campaigns save executive time and uncover hidden accounts at risk before cancellation.
            </CardDescription>
          </DialogHeader>
          <div className="space-y-4 py-3">
            <div>
              <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Campaign Title</Label>
              <Input placeholder="E.g. Q4 Executive Relationship Audit" value={newTitle} onChange={(e) => setNewTitle(e.target.value)} className="mt-1 h-10 text-sm" />
            </div>
            <div>
              <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Description & Incentive</Label>
              <Input placeholder="E.g. Help us optimize your workflow for 2026" value={newDesc} onChange={(e) => setNewDesc(e.target.value)} className="mt-1 h-10 text-sm" />
            </div>
            <div className="grid grid-cols-2 gap-3">
              <div>
                <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Survey Metric</Label>
                <select value={newType} onChange={(e) => setNewType(e.target.value as any)} className="w-full mt-1 h-10 px-3 bg-slate-50 dark:bg-slate-800 border border-slate-200 dark:border-slate-700 rounded-lg text-sm font-medium">
                  <option value="NPS">NPS (Net Promoter)</option>
                  <option value="CSAT">CSAT (Satisfaction)</option>
                  <option value="CES">CES (Effort Score)</option>
                </select>
              </div>
              <div>
                <Label className="text-xs font-semibold text-slate-700 dark:text-slate-300">Target Audience</Label>
                <Input value={newTarget} onChange={(e) => setNewTarget(e.target.value)} className="mt-1 h-10 text-sm" />
              </div>
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" size="sm" onClick={() => setIsNewModalOpen(false)}>Cancel</Button>
            <Button size="sm" onClick={handleCreateCampaign} disabled={!newTitle.trim()} className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold">Launch Draft Campaign</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Live Preview Modal (Simulating Web Widget / Email Pulse) */}
      <Dialog open={isPreviewOpen} onOpenChange={setIsPreviewOpen}>
        <DialogContent className="max-w-md p-6 bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 shadow-2xl">
          <DialogHeader>
            <div className="flex items-center justify-between">
              <Badge className="bg-emerald-100 text-emerald-800 dark:bg-emerald-900/50 dark:text-emerald-300 font-mono text-[10px]">LIVE WIDGET PREVIEW</Badge>
            </div>
            <DialogTitle className="text-xl font-extrabold text-slate-900 dark:text-white mt-2">{activeSurvey?.title}</DialogTitle>
            <CardDescription className="text-xs text-slate-500 dark:text-slate-400">{activeSurvey?.description}</CardDescription>
          </DialogHeader>
          <div className="space-y-6 py-4 max-h-[60vh] overflow-y-auto pr-1">
            {questions.map((q, idx) => (
              <div key={q.id} className="space-y-2">
                <p className="text-sm font-bold text-slate-900 dark:text-white">
                  {idx + 1}. {q.questionText} {q.isRequired && <span className="text-rose-500">*</span>}
                </p>
                {q.questionType === "StarRating" && (
                  <div className="flex items-center gap-2 py-1">
                    {[1, 2, 3, 4, 5].map((s) => (
                      <button key={s} type="button" className="p-2 bg-amber-50 dark:bg-amber-950/30 hover:bg-amber-100 rounded-lg text-amber-500 font-bold transition-all">
                        <Star className="h-6 w-6 fill-current" />
                      </button>
                    ))}
                  </div>
                )}
                {q.questionType === "NpsScale" && (
                  <div className="flex items-center justify-between gap-1 py-1">
                    {[0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map((n) => (
                      <button key={n} type="button" className="w-8 h-8 rounded bg-slate-100 dark:bg-slate-800 hover:bg-indigo-600 hover:text-white text-xs font-bold transition-colors">
                        {n}
                      </button>
                    ))}
                  </div>
                )}
                {q.questionType === "MultipleChoice" && (
                  <div className="space-y-2">
                    {(q.options || ["Yes, resolved completely", "No, needed follow up"]).map((opt, oIdx) => (
                      <div key={oIdx} className="p-2.5 rounded-lg border border-slate-200 dark:border-slate-700 hover:border-indigo-500 cursor-pointer text-xs font-medium text-slate-700 dark:text-slate-300">
                        {opt}
                      </div>
                    ))}
                  </div>
                )}
                {q.questionType === "Text" && (
                  <textarea placeholder="Type your honest feedback here..." className="w-full h-24 p-3 rounded-lg border border-slate-200 dark:border-slate-700 bg-slate-50 dark:bg-slate-800 text-xs" />
                )}
              </div>
            ))}
          </div>
          <DialogFooter>
            <Button size="sm" className="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-bold h-10 shadow-lg" onClick={() => setIsPreviewOpen(false)}>
              Submit Feedback (Simulated)
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
